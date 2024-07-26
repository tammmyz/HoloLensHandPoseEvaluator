import json
import pandas as pd

"""
Class to extract data
"""
def get_ground_truth_data_list(filename):
    df = pd.read_csv(filename)
    return get_gt_list(df)

def get_recorded_data_list(filename, start, n, sampling=3):
    with open(filename, "r") as file:
        data = json.load(file)

    inds = []
    data_list = []
    cols = []
    for key in data:
        if key == "handedness":
            pass
        elif key == "startTime":
            pass
        else:
            inds.append(int(key))
            hand_info = []
            for d in data[key]:
                hand_info.append(data[key][d])
            data_list.append(hand_info)
    for key in data[str(inds[0])]:
        cols.append(key)

    df = pd.DataFrame(data_list, index=inds, columns=cols)
    return extract_points(df, start, n, sampling)

def extract_points(df, start, n, sampling=3):
    """
    Function to extract points from model inference JSON data
    :param df: Pandas Dataframe containing recorded hand coordinates
    :param start: frame count corresponding to start of sample
    :param n: number of frames in sample
    :param sampling: frame sampling rate (based on video processing steps)
    :return: list of 3D coordinates for all joints in the hand, following indexing order for MediaPipe hand landmarks
    """
    inds = [i for i in range(start, start + sampling * n, sampling)]
    df['combined_hand_data'] = df.apply(lambda x: combine_hand_data(
        x["thumb"], x["index"], x["middle"], x["ring"], x["pinky"], x["wrist"]
    ), axis=1)
    return df['combined_hand_data'].loc[inds].tolist()

def format_thumb(x):
    return x[::-1]

def format_finger(x):
    return x[:-1][::-1]

def combine_hand_data(thumb, index, middle, ring, pinky, wrist):
    data = [wrist]
    data += format_thumb(thumb)
    data += format_finger(index)
    data += format_finger(middle)
    data += format_finger(ring)
    data += format_finger(pinky)
    return data

def get_gt_list(df, n_joints=21):
    df_list = []
    for row in df.index:
        joints = df.loc[row].tolist()
        append_joints = []
        for i in range(0, n_joints * 3, 3):
            append_joints.append(joints[i:i+3])
        df_list.append(append_joints)
    return df_list