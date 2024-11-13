# maze.py

import heapq

class Node:
    def __init__(self, position, parent=None, g=0, h=0):
        self.position = position
        self.parent = parent
        self.g = g
        self.h = h
        self.f = g + h

    def __lt__(self, other):
        return self.f < other.f

def astar(maze, start, end):
    open_list = []
    closed_list = set()
    heapq.heappush(open_list, Node(start))

    while open_list:
        current_node = heapq.heappop(open_list)
        if current_node.position == end:
            path = []
            while current_node:
                path.append(current_node.position)
                current_node = current_node.parent
            return path[::-1]

        closed_list.add(current_node.position)

        for neighbor in neighbors(current_node.position, maze):
            if neighbor in closed_list:
                continue
            g = current_node.g + 1
            h = abs(neighbor[0] - end[0]) + abs(neighbor[1] - end[1])
            neighbor_node = Node(neighbor, current_node, g, h)
            heapq.heappush(open_list, neighbor_node)

def neighbors(position, maze):
    x, y = position
    potential_neighbors = [(x-1, y), (x+1, y), (x, y-1), (x, y+1)]
    return [n for n in potential_neighbors if 0 <= n[0] < len(maze) and 0 <= n[1] < len(maze[0]) and maze[n[0]][n[1]] == 0]

# Example maze (1 = wall, 0 = free space)
maze = [
    [1, 0, 1, 1, 0, 1],
    [1, 0, 1, 0, 0, 0],
    [1, 0, 0, 1, 1, 0],
    [0, 0, 1, 1, 0, 1],
    [1, 0, 0, 0, 1, 0],
    [1, 0, 1, 0, 1, 0]
]

start = (0, 1)
end = (5, 4)

# Running the A* algorithm
path = astar(maze, start, end)
print("Path:", path)

