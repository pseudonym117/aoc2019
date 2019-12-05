
from collections import namedtuple
from enum import Enum
from typing import Tuple

Operation = namedtuple('Operation', ['op', 'args'])
Argument = namedtuple('Argument', ['mode', 'val'])

class Ops(Enum):
    ADD = 1
    MULT = 2
    INPUT = 3
    OUTPUT = 4
    JMPE = 5
    JMPNE = 6
    CLT = 7
    CEQ = 8
    STOP = 99

def main():
    input_file = 'input.txt'

    with open(input_file, 'r') as f:
        contents = f.read().split(',')
    prog = [int(c) for c in contents]

    part_1(prog.copy())
    part_2(prog)


def part_1(program: list):
    output = run_program(program, [1, ])

    print(', '.join([str(s) for s in output]))


def part_2(program: list):
    output = run_program(program, [5, ])
    
    print(', '.join([str(s) for s in output]))


def decode_operation(program: list, at: int) -> Tuple[Operation, int]:
    op_raw = program[at]
    
    op_code = Ops(op_raw % 100)

    mod_1 = op_raw // 100 % 10
    mod_2 = op_raw // 1000 % 10
    mod_3 = op_raw // 10000

    # 3 arg calls
    if op_code in (Ops.ADD, Ops.MULT, Ops.CLT, Ops.CEQ, ):
        args = (
            Argument(mod_1, program[at + 1]),
            Argument(mod_2, program[at + 2]),
            Argument(mod_3, program[at + 3]),
        )
        return Operation(op_code, args), at + 4
    
    # 2 arg calls
    if op_code in (Ops.JMPE, Ops.JMPNE):
        args = (
            Argument(mod_1, program[at + 1]),
            Argument(mod_2, program[at + 2]),
        )
        return Operation(op_code, args), at + 3

    # 1 arg calls
    if op_code == Ops.INPUT or op_code == Ops.OUTPUT:
        args = (Argument(mod_1, program[at + 1]), )
        return Operation(op_code, args), at + 2

    if op_code != Ops.STOP:
        raise ValueError('illegal argument')

    return Operation(op_code, []), at + 1


def resolve_argument(program: list, arg: Argument) -> int:
    if arg.mode == 0:
        return program[arg.val]
    elif arg.mode == 1:
        return arg.val
    raise ValueError('illegal mode')


def run_program(program: list, inputs: list) -> list:
    try:
        index = 0
        output = []
        rev_inputs = inputs[::-1] # reverse input so that values can be popped, like a stack
        while index < len(program):
            index = run_op(program, index, rev_inputs, output)
    except StopIteration:
        return output
    assert False


def run_op(program: list, position: int, inputs: list, output: list) -> int:
    op, position = decode_operation(program, position)

    if op.op == Ops.STOP:
        raise StopIteration()

    if op.op in (Ops.ADD, Ops.MULT, Ops.CEQ, Ops.CLT, ):
        operand_1 = resolve_argument(program, op.args[0])
        operand_2 = resolve_argument(program, op.args[1])

        destination = op.args[2].val

        if op.op == Ops.ADD:
            value = operand_1 + operand_2
        elif op.op == Ops.MULT:
            value = operand_1 * operand_2
        elif op.op == Ops.CLT:
            value = 1 if operand_1 < operand_2 else 0
        elif op.op == Ops.CEQ:
            value = 1 if operand_1 == operand_2 else 0

        program[destination] = value

    elif op.op in (Ops.JMPE, Ops.JMPNE, ):
        cmp_val = resolve_argument(program, op.args[0])
        n_position = resolve_argument(program, op.args[1])

        if op.op == Ops.JMPE and cmp_val != 0:
            position = n_position
        elif op.op == Ops.JMPNE and cmp_val == 0:
            position = n_position

    elif op.op == Ops.INPUT:
        destination = op.args[0].val
        value = inputs.pop()

        program[destination] = value
    
    elif op.op == Ops.OUTPUT:
        value = resolve_argument(program, op.args[0])
        
        output.append(value)

    return position


if __name__ == '__main__':
    main()
