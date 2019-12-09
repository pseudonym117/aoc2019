
from enum import Enum
from queue import Queue
from typing import List, Iterator


class Op(Enum):
    ADD = 1
    MULT = 2
    INPUT = 3
    OUTPUT = 4
    JMPE = 5
    JMPNE = 6
    CLT = 7
    CEQ = 8
    MRB = 9
    STOP = 99


class Mode(Enum):
    POINTER = 0
    IMMEDIATE = 1
    RELATIVE = 2


class Argument:
    def __init__(self, program, mode: Mode, value: int):
        self._program = program
        self._mode = mode
        self._value = value
    
    def resolve(self):
        if self._mode == Mode.POINTER:
            return self._program[self._value]
        elif self._mode == Mode.IMMEDIATE:
            return self._value
        elif self._mode == Mode.RELATIVE:
            return self._program[self._program.relative_base + self._value]
        raise ValueError('illegal mode')

    def set_value(self, value: int):
        if self._mode == Mode.RELATIVE:
            addr = self._program.relative_base + self._value
        else:
            addr = self._value

        self._program[addr] = value


class Operation:
    def __init__(self, program, args: List[Argument]):
        self._args = args
        self._program = program
    
    @staticmethod
    def create(program, op: Op, args: List[Argument]):
        clsmap = {
            Op.ADD: AddOperation,
            Op.MULT: MultOperation,
            Op.INPUT: InputOperation,
            Op.OUTPUT: OutputOperation,
            Op.JMPE: JumpEqualOperation,
            Op.JMPNE: JumpNotEqualOperation,
            Op.CLT: CompareLessThanOperation,
            Op.CEQ: CompareEqualOperation,
            Op.MRB: ModifyRelativeBase,
            Op.STOP: StopOperation,
        }

        return clsmap[op](program, args)


class StopOperation(Operation):
    def __call__(self):
        raise StopIteration()


class AddOperation(Operation):
    def __call__(self):
        value = self._args[0].resolve() + self._args[1].resolve()
        self._args[2].set_value(value)


class MultOperation(Operation):
    def __call__(self):
        value = self._args[0].resolve() * self._args[1].resolve()
        self._args[2].set_value(value)


class CompareEqualOperation(Operation):
    def __call__(self):
        value = 1 if self._args[0].resolve() == self._args[1].resolve() else 0
        self._args[2].set_value(value)


class CompareLessThanOperation(Operation):
    def __call__(self):
        value = 1 if self._args[0].resolve() < self._args[1].resolve() else 0
        self._args[2].set_value(value)


class JumpEqualOperation(Operation):
    def __call__(self):
        if self._args[0].resolve() != 0:
            self._program._eip = self._args[1].resolve()


class JumpNotEqualOperation(Operation):
    def __call__(self):
        if self._args[0].resolve() == 0:
            self._program._eip = self._args[1].resolve()


class InputOperation(Operation):
    def __call__(self):
        value = self._program.inputs.get()
        self._args[0].set_value(value)


class OutputOperation(Operation):
    def __call__(self):
        value = self._args[0].resolve()
        self._program.output.put(value)


class ModifyRelativeBase(Operation):
    def __call__(self):
        value = self._args[0].resolve()
        self._program.relative_base += value


class Program:
    def __init__(self, code: List[int], inputs: Queue, output: Queue):
        self._code = code
        self._eip = 0
        self.inputs = inputs
        self.output = output

        self.relative_base = 0
    
    def __iter__(self) -> Iterator[Operation]:
        while self._eip < len(self._code):
            yield self._consume_next_operation()

    def run(self):
        try:
            for op in self:
                op()
        except StopIteration:
            return
        assert False

    def __getitem__(self, key: int) -> int:
        self._ensure_addr(key)
        return self._code[key]
    
    def __setitem__(self, key: int, value: int):
        self._ensure_addr(key)
        self._code[key] = value

    def _ensure_addr(self, addr):
        while len(self._code) <= addr:
            self._code.append(0)

    def _consume_argument(self, modifier) -> Argument:
        arg = Argument(self, modifier, self[self._eip])
        self._eip += 1
        return arg

    def _consume_next_operation(self) -> Operation:
        op_raw = self[self._eip]
        self._eip += 1
    
        op_code = Op(op_raw % 100)

        args = []
        if op_code == Op.STOP:
            return Operation.create(self, op_code, args)

        mod_1 = Mode(op_raw // 100 % 10)
        args.append(self._consume_argument(mod_1))

        # 1 arg calls
        if op_code == Op.INPUT or op_code == Op.OUTPUT or op_code == Op.MRB:
            return Operation.create(self, op_code, args)

        mod_2 = Mode(op_raw // 1000 % 10)
        args.append(self._consume_argument(mod_2))

        # 2 arg calls
        if op_code in (Op.JMPE, Op.JMPNE):
            return Operation.create(self, op_code, args)

        mod_3 = Mode(op_raw // 10000)
        args.append(self._consume_argument(mod_3))

        # 3 arg calls
        if op_code in (Op.ADD, Op.MULT, Op.CLT, Op.CEQ, ):
            return Operation.create(self, op_code, args)
        
        raise ValueError('illegal argument')
