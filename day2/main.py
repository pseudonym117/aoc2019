
def main():
    input_file = 'input.txt'

    with open(input_file, 'r') as f:
        contents = f.read().split(',')
    prog = [int(c) for c in contents]

    part1_run(prog.copy())

    part2_brute_force(prog)
    
    
def part1_run(program: list):
    program[1] = 12
    program[2] = 2

    run_program(program)
    
    print(f"position 0: {program[0]}")


def part2_brute_force(program: list):
    found = False
    for verb in range(len(program)):
        if found:
            break
        for noun in range(len(program)):
            n_prog = program.copy()

            n_prog[1] = noun
            n_prog[2] = verb

            try:
                run_program(n_prog)
            except:
                continue

            if n_prog[0] == 19690720:
                found = True
                print(f"noun: {noun}, verb: {verb}")
                print(f"answer: {100 * noun + verb}")
                break
    if not found:
        print("part 2 brute force completed without answer")


def run_program(program: list):
    try:
        index = 0
        while index < len(program):
            index = run_op(program, index)
    except StopIteration:
        return
    assert False

def run_op(program: list, position: int) -> int:
    ops = (1, 2, 99, )

    op = program[position]

    assert op in ops

    if op == 99:
        raise StopIteration()

    operand_location_1 = program[position + 1]
    operand_location_2 = program[position + 2]
    destination = program[position + 3]

    operand_1 = program[operand_location_1]
    operand_2 = program[operand_location_2]

    value = operand_1 + operand_2 if op == 1 else operand_1 * operand_2

    program[destination] = value

    return position + 4


if __name__ == '__main__':
    main()
