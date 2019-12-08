
from itertools import permutations
from queue import Queue
from threading import Thread

from day7 import Program


def main():
    input_file = 'input.txt'

    with open(input_file, 'r') as f:
        contents = f.read().split(',')
    prog = [int(c) for c in contents]

    part_1(prog.copy())
    part_2(prog)


def make_stream(*args) -> Queue:
    stream = Queue()
    for arg in args:
        stream.put(arg)
    return stream


def part_1(code: list):
    max_power = 0
    max_order = None

    for order in permutations(range(0, 5)):
        power = 0
        for phase in order:
            input_stream = make_stream(phase, power)
            output_stream = make_stream()

            prog = Program(code.copy(), input_stream, output_stream)
            prog.run()
            power = output_stream.get()
        
        if power > max_power:
            max_power = power
            max_order = order

    print(f'maxiumum power: {max_power} from inputs: {", ".join([str(m) for m in max_order])}')


def part_2(code: list):
    max_power = 0
    max_order = None

    for order in permutations(range(5, 10)):
        streams = [make_stream(phase) for phase in order]
        streams[0].put(0)

        progs = [
            Program(code.copy(), streams[i], streams[(i + 1) % len(streams)])
            for i in range(len(streams))
        ]

        threads = [
            start_program_thread(prog)
            for prog in progs
        ]

        for th in threads:
            th.join()
        
        power = streams[0].get()
        
        if power > max_power:
            max_power = power
            max_order = order

    print(f'maxiumum power: {max_power} from inputs: {", ".join([str(m) for m in max_order])}')


def start_program_thread(program: Program) -> Thread:
    th = Thread(target=lambda prog: prog.run(), args=(program, ))
    th.start()
    return th


if __name__ == '__main__':
    main()
