import math
import matplotlib.colors as mcolors
import matplotlib.pyplot as plt


def view_diff_matrix(diff_arr, title, cmap, norm):
    fig, ax = plt.subplots(figsize=(30, 10))

    # Transpose the array to rotate the graph 90 degrees
    diff_arr = diff_arr.T
    cax = ax.matshow(diff_arr, cmap=cmap, norm=norm)
    cbar = fig.colorbar(cax, ax=ax, fraction=0.046, pad=0.04)
    cbar.set_label('Angle Difference (Degrees)', labelpad=15, fontsize=16)
    cbar.ax.tick_params(labelsize=14)
    cbar.ax.yaxis.label.set_rotation(270)

    ax.tick_params(axis='x', labelsize=16, labelbottom=True, labeltop=False)
    ax.tick_params(axis='y', labelrotation=45, labelsize=16)

    ylabels = []
    for finger in ["thumb", "index", "middle", "ring", "pinky"]:
        ylabels.append(f"{finger}-knuckle")
        ylabels.append(f"{finger}-middle")
        ylabels.append(f"{finger}-tip")

    ax.set_yticks(range(len(ylabels)))
    ax.set_yticklabels(ylabels, rotation=45, ha='right')

    ax.set_xlabel("Frame Number", labelpad=20, fontsize=18)
    ax.set_ylabel("Joint", labelpad=15, fontsize=16)
    ax.set_title(f"{title}", fontsize=18)

    plt.show()

def calculate_overall_hand_diff(hand_1, hand_2):
    diffs = calculate_finger_diff(hand_1, hand_2, 0, 1, 4)
    diffs += calculate_finger_diff(hand_1, hand_2, 0, 5, 8)
    diffs += calculate_finger_diff(hand_1, hand_2, 0, 9, 12)
    diffs += calculate_finger_diff(hand_1, hand_2, 0, 13, 16)
    diffs += calculate_finger_diff(hand_1, hand_2, 0, 17, 20)
    return sum(diffs)/len(diffs), diffs
def calculate_finger_diff(hand_1, hand_2, wrist, i_start, i_end):
    diffs = [calculate_angle_diff(hand_1, hand_2, wrist, i_start, i_start + 1)]
    for i in range(i_start, i_end - 1):
        diffs.append(calculate_angle_diff(hand_1, hand_2, i, i + 1, i + 2))
    return diffs
def calculate_angle_diff(hand_1, hand_2, a, b, c):
    angle_1 = calculate_angle(hand_1[a], hand_1[b], hand_1[c])
    angle_2 = calculate_angle(hand_2[a], hand_2[b], hand_2[c])
    return abs(angle_1 - angle_2)
def calculate_angle(a, b, c):
    dABx = a[0] - b[0]
    dABy = a[1] - b[1]
    dABz = a[2] - b[2]

    dBCx = b[0] - c[0]
    dBCy = b[1] - c[1]
    dBCz = b[2] - c[2]

    dot_product = dABx * dBCx + dABy * dBCy + dABz * dBCz
    magAB = dABx ** 2 + dABy ** 2 + dABz ** 2
    magBC = dBCx ** 2 + dBCy ** 2 + dBCz ** 2

    cosine_angle = dot_product / math.sqrt(magAB * magBC)
    angle_deg = math.acos(cosine_angle) * 180 / math.pi

    return angle_deg
