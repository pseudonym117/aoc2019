
from collections import namedtuple
from typing import Iterator, Iterable, Union


Point = namedtuple('Point', ['x', 'y'])
Segment = namedtuple('Segment', ['p1', 'p2'])


def get_segment_intersection(segment1: Segment, segment2: Segment) -> Union[Point, None]:
    """
    https://stackoverflow.com/a/1968345
    """
    
    s1 = Point(segment1.p2.x - segment1.p1.x, segment1.p2.y - segment1.p1.y)
    s2 = Point(segment2.p2.x - segment2.p1.x, segment2.p2.y - segment2.p1.y)

    denom = (-s2.x * s1.y + s1.x * s2.y)
    if denom == 0:
        return None

    s = (-s1.y * (segment1.p1.x - segment2.p1.x) + s1.x * (segment1.p1.y - segment2.p1.y)) / denom
    t = ( s2.x * (segment1.p1.y - segment2.p1.y) - s2.y * (segment1.p1.x - segment2.p1.x)) / denom

    if 0 <= s and s <= 1 and 0 <= t and t <= 1:
        return Point(segment1.p1.x + (t * s1.x), segment1.p1.y + (t * s1.y))
    
    return None


def get_all_line_intersections(line1: Iterable[Segment], line2: Iterable[Segment]) -> Iterator[Point]:
    for segment1 in line1:
        for segment2 in line2:
            intersect = get_segment_intersection(segment1, segment2)
            if intersect:
                yield intersect


def is_point_on_segment(point: Point, segment: Segment) -> bool:
    """
    https://stackoverflow.com/a/328122
    """
    crossproduct = (point.y - segment.p1.y) * (segment.p2.x - segment.p1.x) - (point.x - segment.p1.x) * (segment.p2.y - segment.p1.y)

    if crossproduct != 0:
        return False

    dotproduct = (point.x - segment.p1.x) * (segment.p2.x - segment.p1.x) + (point.y - segment.p1.y) * (segment.p2.y - segment.p1.y)

    if dotproduct < 0:
        return False
    
    squared_length_ba = (segment.p2.x - segment.p1.x)**2 + (segment.p2.y - segment.p1.y)**2

    if dotproduct > squared_length_ba:
        return False

    return True


def ops_to_lines(line_ops: Iterable[str]) -> Iterator[Segment]:
    at = Point(0, 0)

    for op in line_ops:
        direction, distance = op[0], int(op[1:])

        if direction == 'U':
            new_point = Point(at.x, at.y + distance)
        elif direction == 'D':
            new_point = Point(at.x, at.y - distance)
        elif direction == 'L':
            new_point = Point(at.x - distance, at.y)
        elif direction == 'R':
            new_point = Point(at.x + distance, at.y)
        else:
            raise ValueError(f'invalid direction: "{direction}"')

        segment = Segment(at, new_point)
        at = new_point

        yield segment


def line_distance_to_point(line: Iterable[Segment], point: Point) -> Union[int, None]:
    total = 0

    for seg in line:
        if is_point_on_segment(point, seg):
            total += abs(seg.p1.x - point.x) + abs(seg.p1.y - point.y)
            return total
        else:
            # since this is a straight line, we can cheat here and not use pythagorean theorum
            total += abs(seg.p1.x - seg.p2.x) + abs(seg.p1.y - seg.p2.y)
    
    return None


def main():
    input_file = 'input.txt'

    with open(input_file, 'r') as f:
        lines_defs = f.readlines()
    line_ops = [l.split(',') for l in lines_defs]

    lines = [list(ops_to_lines(op)) for op in line_ops]

    line1, line2 = lines

    intersects = [p for p in get_all_line_intersections(line1, line2) if p.x != 0 and p.y != 0]

    # part 1
    distances = [p.x + p.y for p in intersects]
    print(f'closest intersection is {min(distances)} away')

    # part 2
    distances = [
        line_distance_to_point(line1, p) + line_distance_to_point(line2, p)
        for p in intersects
    ]
    print(f'closest intersection by travel distance is {min(distances)}')


if __name__ == '__main__':
    main()
