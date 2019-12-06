
class Tree:
    def __init__(self, data):
        self.data = data
        self.connections = []
    
    def __contains__(self, node):
        if self.data == node:
            return True
        
        for conn in self.connections:
            if node in conn:
                return True
        
        return False
    
    def dfs(self):
        for conn in self.connections:
            for c2 in conn.dfs():
                yield c2
        yield self
    
    def find_depth(self, value):
        if self.data == value:
            return 0

        return 1 + sum([c.find_depth(value) for c in self.connections if value in c])


def main():
    input_file = 'input.txt'

    with open(input_file, 'r') as f:
        orbits = [orb.strip().split(')') for orb in f.readlines()]
    
    root = Tree('COM')

    build_tree(orbits, root)

    part_1(root)
    part_2(root)


def part_1(tree):
    print(count_overall_depth(tree))


def part_2(tree):
    you = 'YOU'
    santa = 'SAN'

    common_parent = None
    for n in tree.dfs():
        if you in n and santa in n:
            common_parent = n
            break
    
    assert common_parent is not None

    santa_track = [c for c in common_parent.connections if santa in c][0]
    you_track = [c for c in common_parent.connections if you in c][0]

    you_depth = you_track.find_depth(you)
    santa_depth = santa_track.find_depth(santa)

    print(f'distance between you and santa is {you_depth + santa_depth}')


def build_tree(orbits, node):
    node.connections = [Tree(orb) for src, orb in orbits if src == node.data]
    
    for conn in node.connections:
        build_tree(orbits, conn)


def count_overall_depth(tree):
    def _count_overall_depth_helper(node, current):
        chidren = sum([_count_overall_depth_helper(c, current + 1) for c in node.connections])

        return current + chidren
        
    return _count_overall_depth_helper(tree, 0)


if __name__ == '__main__':
    main()
