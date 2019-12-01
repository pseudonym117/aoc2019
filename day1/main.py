
def main():
    input_file = 'input.txt'

    with open(input_file, 'r') as f:
        fuel_func = lambda mass: max(mass // 3 - 2, 0)

        fuel_as_int = [int(mass) for mass in f.readlines()]

    fuel_per_mod = [fuel_func(mass) for mass in fuel_as_int]

    fuel = sum(fuel_per_mod)
    print(fuel)

    # end part 1

    while True:
        old_fuel = fuel
        fuel_per_mod = [fuel_func(f) for f in fuel_per_mod]
        fuel += sum(fuel_per_mod)
        if old_fuel == fuel:
            break

    print(fuel)

if __name__ == '__main__':
    main()
