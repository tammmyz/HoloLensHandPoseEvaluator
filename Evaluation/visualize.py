import matplotlib.pyplot as plt

def visualize_hand(hand, style='b', elev=-140, azims=[0, 60, 120, 180], figsize=(15, 20)):
    fig = plt.figure(figsize=figsize)
    for (i, azim) in enumerate(azims):
        ax = fig.add_subplot(1, len(azims), i + 1, projection='3d')
        draw_hand(hand, ax, elev, azim, style)

def draw_hand(vecs, ax, elev, azim, style='b'):
    """
    Function to draw hand based on 21 landmark model
    :param ax:
    :param elev:
    :param azim:
    :return:
    """
    draw_finger(0, 1, 4, vecs, ax, style)
    draw_finger(0, 5, 8, vecs, ax, style)
    draw_finger(0, 9, 12, vecs, ax, style)
    draw_finger(0, 13, 16, vecs, ax, style)
    draw_finger(0, 17, 20, vecs, ax, style)
    ax.view_init(elev, azim)

def draw_hand_mrtk(vecs, ax, elev, azim, style='b'):
    """
    Function to draw hand based on 25 landmark model from MRTK
    :param ax:
    :param elev:
    :param azim:
    :return:
    """
    draw_finger(0, 1, 4, vecs, ax, style)
    draw_finger(0, 5, 9, vecs, ax, style)
    draw_finger(0, 10, 14, vecs, ax, style)
    draw_finger(0, 15, 19, vecs, ax, style)
    draw_finger(0, 20, 24, vecs, ax, style)
    ax.view_init(elev, azim)

def draw_finger(i_wrist, i_finger_start, i_finger_end, vecs, ax, style='b'):
    plot_line(vecs, i_wrist, i_finger_start, ax, style)
    for i in range(i_finger_start, i_finger_end):
        plot_line(vecs, i, i + 1, ax, style)

def plot_line(points, i_start, i_end, ax, style='b'):
    """
    Plots line in 3D space
    :param points: list of coordinates in 3D space
    :param i_start: index corresponding to starting vertex of line
    :param i_end: index corresponding to ending vertex of line
    :param ax: PyPlot axes object
    :return: None
    """
    start = points[i_start]
    end = points[i_end]
    ax.plot([start[0], end[0]], [start[1], end[1]], [start[2], end[2]], style)
