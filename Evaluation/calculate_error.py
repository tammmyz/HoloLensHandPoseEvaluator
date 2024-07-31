import math
import matplotlib.colors as mcolors
import matplotlib.pyplot as plt
import numpy as np

NORM_A = mcolors.Normalize(vmin=0, vmax=120)
NORM_D = mcolors.Normalize(vmin=0, vmax=300)

def view_angle_matrix(diff_arr, title, cmap, norm=NORM_A):
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

def view_distance_matrix(diff_arr, title, cmap, norm=NORM_D):
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

    ylabels = ["wrist", "thumb-cmc", "thumb-mcp", "thumb-ip", "thumb-tip"]
    for finger in ["index", "middle", "ring", "pinky"]:
        ylabels.append(f"{finger}-mcp")
        ylabels.append(f"{finger}-pip")
        ylabels.append(f"{finger}-dip")
        ylabels.append(f"{finger}-tip")

    ax.set_yticks(range(len(ylabels)))
    ax.set_yticklabels(ylabels, rotation=45, ha='right')

    ax.set_xlabel("Frame Number", labelpad=20, fontsize=18)
    ax.set_ylabel("Joint", labelpad=15, fontsize=16)
    ax.set_title(f"{title}", fontsize=18)

    plt.show()

def angle_diff_summary(gt, data):
    avg_diffs = []
    angle_diffs = []
    for i in range(len(gt)):
        avg, angle = calculate_overall_hand_angles(gt[i], data[i])
        avg_diffs.append(avg)
        angle_diffs.append(angle)
    A = np.array(angle_diffs)

    print(f"Max hand avg angle difference:\t{round(max(avg_diffs), 2)}")
    print(f"Min hand avg angle difference:\t{round(min(avg_diffs), 2)}")
    print(f"Avg hand avg angle difference:\t{round(sum(avg_diffs) / len(avg_diffs), 2)}")
    print(f"Overall max angle difference: \t{round(A.max(), 2)}")
    print(f"Overall min angle difference: \t{round(A.min(), 2)}")
    return A

def dist_diff_summary(gt, data):
    avg_dists = []
    all_dists = []

    for i in range(len(gt)):
        avg, dists = calculate_overall_distances(gt[i], data[i])
        avg_dists.append(avg)
        all_dists.append(dists)
    D = np.array(all_dists)

    print(f"Max hand avg position difference:\t{round(max(avg_dists), 2)}")
    print(f"Min hand avg position difference:\t{round(min(avg_dists), 2)}")
    print(f"Avg hand avg position difference:\t{round(sum(avg_dists) / len(avg_dists), 2)}")
    print(f"Overall max position difference: \t{round(D.max(), 2)}")
    print(f"Overall min position difference: \t{round(D.min(), 2)}")
    return D

def calculate_overall_hand_angles(hand_1, hand_2):
    diffs = calculate_finger_diff(hand_1, hand_2, 0, 1, 4)
    diffs += calculate_finger_diff(hand_1, hand_2, 0, 5, 8)
    diffs += calculate_finger_diff(hand_1, hand_2, 0, 9, 12)
    diffs += calculate_finger_diff(hand_1, hand_2, 0, 13, 16)
    diffs += calculate_finger_diff(hand_1, hand_2, 0, 17, 20)
    return sum(diffs)/len(diffs), diffs

def calculate_overall_distances(hand_1, hand_2):
    dists = []
    for i in range(len(hand_1)):
        dists.append(np.linalg.norm(hand_1[i] - hand_2[i]))
    return sum(dists)/len(dists), dists

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

def check_hand_equality(hand_arr, t_hand_arr):
    d_diffs = []
    ang_diffs = []
    for i in range(len(hand_arr)):
        temp = calculate_consecutive_joint_dists(hand_arr[i], t_hand_arr[i])
        _, ang_list = calculate_overall_hand_angles(hand_arr[i], t_hand_arr[i])
        d_diffs.append(sum(temp))
        ang_diffs.append(sum(ang_list))
    print('total distance differences: ', sum(d_diffs))
    print('total angle differences: ', sum(ang_diffs))
    return d_diffs, ang_diffs

def calculate_consecutive_joint_dists(hand, t_hand):
    hand_dists = []
    t_hand_dists = []
    for i in range(1, len(hand)):
        hand_dists.append(np.linalg.norm(hand[i] - hand[i-1]))
        t_hand_dists.append(np.linalg.norm(t_hand[i] - t_hand[i-1]))
    diffs = []
    for i in range(len(hand_dists)):
        diffs.append(abs(hand_dists[i] - t_hand_dists[i]))
    return diffs