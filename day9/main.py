
from itertools import permutations
from queue import Queue
from threading import Thread

from day9 import Program


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
    input_stream = make_stream(1)
    output_stream = make_stream()

    prog = Program(code, input_stream, output_stream)
    prog.run()

    output = []
    while not output_stream.empty():
        output.append(output_stream.get())

    print(f'part 1 output: {", ".join([str(i) for i in output])}')


def part_2(code: list):
    input_stream = make_stream(2)
    output_stream = make_stream()

    prog = Program(code, input_stream, output_stream)
    prog.run()

    output = []
    while not output_stream.empty():
        output.append(output_stream.get())

    print(f'part 2 output: {", ".join([str(i) for i in output])}')


def start_program_thread(program: Program) -> Thread:
    th = Thread(target=lambda prog: prog.run(), args=(program, ))
    th.start()
    return th


if __name__ == '__main__':
    main()
