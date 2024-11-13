# main.py

import pygame
from pygame.locals import *
from OpenGL.GL import *
from OpenGL.GLUT import *
from OpenGL.GLU import *
from maze import astar

# 3D Cube to represent a wall
def draw_cube():
    vertices = [
        (1, -1, -1),
        (1, 1, -1),
        (-1, 1, -1),
        (-1, -1, -1),
        (1, -1, 1),
        (1, 1, 1),
        (-1, -1, 1),
        (-1, 1, 1)
    ]
    edges = [
        (0, 1),
        (1, 2),
        (2, 3),
        (3, 0),
        (4, 5),
        (5, 6),
        (6, 7),
        (7, 4),
        (0, 4),
        (1, 5),
        (2, 6),
        (3, 7)
    ]
    for edge in edges:
        glBegin(GL_LINES)
        for vertex in edge:
            glVertex3fv(vertices[vertex])
        glEnd()

# Render the entire maze (from maze.py) in 3D
def draw_maze(maze):
    for z in range(len(maze)):
        for x in range(len(maze[z])):
            if maze[z][x] == 1:
                glPushMatrix()
                glTranslatef(x * 2, 0, z * 2)  # Position cubes
                draw_cube()  # Draw cube for wall
                glPopMatrix()

# Render the player (cube moving along the path)
def render_player(path):
    for step in path:
        glPushMatrix()
        glTranslatef(step[1] * 2, 0, step[0] * 2)  # Move player (cube)
        draw_cube()  # Draw player cube
        glPopMatrix()

# Main function to run the game
def main():
    pygame.init()
    display = (800, 600)
    pygame.display.set_mode(display, DOUBLEBUF | OPENGL)
    gluPerspective(45, (display[0] / display[1]), 0.1, 50.0)
    glTranslatef(0.0, 0.0, -15)

    # Maze
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

    path = astar(maze, start, end)
    if path is None:
        print("No valid path found!")
        pygame.quit()
        quit()

    print("Path:", path)

    while True:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                pygame.quit()
                quit()

        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT)
        draw_maze(maze)  # Draw the maze
        render_player(path)  # Render the path taken by the player
        pygame.display.flip()
        pygame.time.wait(10)

if __name__ == "__main__":
    main()
