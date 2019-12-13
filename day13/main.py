
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


class Tile:
    EMPTY = 0
    WALL = 1
    BLOCK = 2
    HOR_PADDLE = 3
    BALL = 4


class Joystick:
    def __init__(self, default_value=0):
        self._default = default_value
        self._val = self._default
    
    def get(self):
        val = self._val
        self._val = self._default
        return val
    
    def put(self, value):
        self._val = value


def make_stream(*args) -> Queue:
    stream = Queue()
    for arg in args:
        stream.put(arg)
    return stream


def run_game(code: list) -> Dict[Tuple[int, int], List[int]]:
    input_stream = make_stream()
    output_stream = Queue(3)

    screen = {}

    prog = Program(code, input_stream, output_stream)

    for op in prog:
        try:
            op()
        except StopIteration:
            break
            
        if output_stream.full():
            x = output_stream.get()
            y = output_stream.get()
            tile_id = output_stream.get()

            pos = x, y

            screen[pos] = tile_id
    
    return screen


def direction_of_ball(ball_location: Tuple[int, int], paddle: Tuple[int, int]) -> int:
    if ball_location[0] < paddle[0]:
        return -1
    elif ball_location[0] > paddle[0]:
        return 1
    return 0


def run_game_with_ai(code: list) -> Dict[Tuple[int, int], int]:
    input_stream = Joystick(default_value=0)
    output_stream = Queue(3)

    screen = {}

    prog = Program(code, input_stream, output_stream)

    paddle_location = None
    for op in prog:
        try:
            op()
        except StopIteration:
            break

        if output_stream.full():
            x = output_stream.get()
            y = output_stream.get()
            tile_id = output_stream.get()

            pos = x, y

            screen[pos] = tile_id

            if tile_id == Tile.HOR_PADDLE:
                paddle_location = pos

            if tile_id == Tile.BALL and paddle_location:
                next_direction = direction_of_ball(pos, paddle_location)
                input_stream.put(next_direction)
    
    return screen

def part_1(code: list):
    end_screen = run_game(code)

    print(f'{len([1 for key, val in end_screen.items() if val == Tile.BLOCK])} block tiles painted at least once')


def part_2(code: list):
    code[0] = 2

    end_screen = run_game_with_ai(code)
    assert (-1, 0) in end_screen

    print(f'final score: {end_screen[(-1, 0)]}')
               

def start_program_thread(program: Program) -> Thread:
    th = Thread(target=lambda prog: prog.run(), args=(program, ))
    th.start()
    return th


if __name__ == '__main__':
    main()
