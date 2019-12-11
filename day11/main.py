
from itertools import permutations
from queue import Queue
from threading import Thread
from typing import Dict, List, Tuple

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


class Direction:
    UP = 0
    RIGHT = 1
    DOWN = 2
    LEFT = 3

class Turn:
    LEFT = 0
    RIGHT = 1

class Paint:
    BLACK = 0
    WHITE = 1

def run_painter(code: list, input_stream: Queue) -> Dict[Tuple[int, int], List[int]]:
    output_stream = Queue(2)
    position = (0, 0)

    painted = {(0, 0): []}
    direction = Direction.UP

    turn_map = {
        Direction.UP: {
            Turn.LEFT: Direction.LEFT,
            Turn.RIGHT: Direction.RIGHT,
        },
        Direction.RIGHT: {
            Turn.LEFT: Direction.UP,
            Turn.RIGHT: Direction.DOWN,
        },
        Direction.DOWN: {
            Turn.LEFT: Direction.RIGHT,
            Turn.RIGHT: Direction.LEFT,
        },
        Direction.LEFT: {
            Turn.LEFT: Direction.DOWN,
            Turn.RIGHT: Direction.UP,
        },
    }

    prog = Program(code, input_stream, output_stream)

    for op in prog:
        
        try:
            op()
        except StopIteration:
            break
            
        if output_stream.full():
            color = output_stream.get()
            turn = output_stream.get()

            if position not in painted:
                painted[position] = []
            painted[position].append(color)

            direction = turn_map[direction][turn]

            if direction == Direction.UP:
                position = (position[0], position[1] - 1)
            elif direction == Direction.DOWN:
                position = (position[0], position[1] + 1)
            elif direction == Direction.LEFT:
                position = (position[0] - 1, position[1])
            elif direction == Direction.RIGHT:
                position = (position[0] + 1, position[1])
            else:
                raise ValueError('invalid direction')

            if position not in painted:
                painted[position] = []
            if painted[position]:
                input_stream.put(painted[position][-1])
            else:
                input_stream.put(Paint.BLACK)
    
    return painted

def part_1(code: list):
    input_stream = make_stream(Paint.BLACK)

    painted = run_painter(code, input_stream)

    print(f'{len(painted)} tiles painted at least once')


def part_2(code: list):
    input_stream = make_stream(Paint.WHITE)

    painted = run_painter(code, input_stream)

    min_x, min_y = 10000000, 10000000
    max_x, max_y = -10000000, -10000000

    for key in painted.keys():
        min_x = min(min_x, key[0])
        min_y = min(min_y, key[1])
        max_x = max(max_x, key[0])
        max_y = max(max_y, key[1])

    print('part 2:')
    for y in range(min_y, max_y + 1):
        for x in range(min_x, max_x + 1):
            colors = painted.get((x, y), [])

            print('█' if colors and colors[-1] == Paint.WHITE else ' ', end='')
        print()
               

def start_program_thread(program: Program) -> Thread:
    th = Thread(target=lambda prog: prog.run(), args=(program, ))
    th.start()
    return th


if __name__ == '__main__':
    main()
