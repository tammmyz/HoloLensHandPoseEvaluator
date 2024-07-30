import json
import pandas as pd
import numpy as np

"""
Class to extract data
"""
def get_ground_truth_data(filename):
    df = pd.read_csv(filename)
    arr = np.array(get_gt_list(df))
    return arr, df

def get_recorded_data(filename, start, n, sampling=3):
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
    arr = np.array(extract_points(df, start, n, sampling))
    return arr, df

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

def transform_data(data_arr, convert_to_mm=True):
    if convert_to_mm:
        # Convert units to mm
        data_arr *= 1000

    # Translate points to wrist of every hand
    trans_data_arr = np.zeros(data_arr.shape)
    for (i, hand) in enumerate(data_arr):
        trans_data_arr[i] = hand - hand[0]

    for i in range(data_arr.shape[0]):
        trans_data_arr[i] = rotate_hand(trans_data_arr[i])

    return trans_data_arr

def rotate_hand(hand):
    wrist = hand[0]
    index_knuckle = hand[15]
    pinky_knuckle = hand[17]

    # Calculate vectors in the plane
    v1 = index_knuckle - wrist
    v2 = pinky_knuckle - wrist

    # Calculate the normal vector of the plane
    normal_vector = np.cross(v1, v2)
    normal_vector = normal_vector / np.linalg.norm(normal_vector)  # Normalize

    target_normal = np.array([0, 1, 0])

    # Calculate the rotation axis (cross product)
    rotation_axis = np.cross(normal_vector, target_normal)
    rotation_axis = rotation_axis / np.linalg.norm(rotation_axis)  # Normalize

    # Calculate the angle between the normal vectors
    angle = np.arccos(np.dot(normal_vector, target_normal))

    R = rotation_matrix(rotation_axis, angle)

    # Apply to all joints
    rotated_joints = np.dot(hand - wrist, R.T) + wrist
    return rotated_joints

def rotation_matrix(axis, theta):
    """
    Return the rotation matrix associated with counterclockwise rotation about
    the given axis by theta radians.
    """
    axis = np.asarray(axis)
    axis = axis / np.sqrt(np.dot(axis, axis))
    a = np.cos(theta / 2.0)
    b, c, d = -axis * np.sin(theta / 2.0)
    aa, bb, cc, dd = a * a, b * b, c * c, d * d
    bc, ad, ac, ab, bd, cd = b * c, a * d, a * c, a * b, b * d, c * d
    return np.array([[aa + bb - cc - dd, 2 * (bc + ad), 2 * (bd - ac)],
                     [2 * (bc - ad), aa + cc - bb - dd, 2 * (cd + ab)],
                     [2 * (bd + ac), 2 * (cd - ab), aa + dd - bb - cc]])
