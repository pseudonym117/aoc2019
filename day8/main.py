
from functools import reduce
from typing import List

def main():
    input_file = 'input.txt'

    with open(input_file, 'r') as f:
        raw = f.read()
    
    part_1(raw)
    part_2(raw)

def read_image(raw, width, height) -> List[List[List[int]]]:
    layer_size = width * height

    layers = []

    for i, c in enumerate(raw):
        if i % layer_size == 0:
            layers.append([])
        
        if i % width == 0:
            layers[-1].append([])
        
        layers[-1][-1].append(int(c))

    return layers


def ns_in_layer(layer: List[List[int]], n: int) -> int:
    return sum(
        [
            sum([1 for col in row if col == n])
            for row in layer
        ]
    )


def part_1(raw_image):
    image = read_image(raw_image, 25, 6)

    zero_layer = None
    zeros = 999999999999
    for layer in image:
        layer_zeros = ns_in_layer(layer, 0)
        if layer_zeros < zeros:
            zeros = layer_zeros
            zero_layer = layer
    
    ones = ns_in_layer(zero_layer, 1)
    twos = ns_in_layer(zero_layer, 2)

    print(f'part 1: {ones * twos}')


def flatten_image(top: List[List[int]], bottom: List[List[int]]) -> List[List[int]]:
    return [
        [
            cols[0] if cols[0] != 2 else cols[1]
            for cols in zip(top[i], bottom[i])
        ]
        for i in range(len(top))
    ]
            


def part_2(raw_image):
    image = read_image(raw_image, 25, 6)

    final = reduce(flatten_image, image)

    translated = [
        ''.join([' ' if c == 0 else '█' for c in row])
        for row in final
    ]

    print('part 2:')

    for row in translated:
        print(row)


if __name__ == '__main__':
    main()
