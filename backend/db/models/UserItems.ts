import mongoose, { Document, Schema } from "mongoose";

export interface IUserItems extends Document {
    user: mongoose.Types.ObjectId;
    coinAmount: Number;
    rewards: String[];
}

const UserItemsSchema: Schema = new mongoose.Schema({
    user: {type: mongoose.Types.ObjectId, ref: "User"},
    coinAmount: { type: Number },
    rewards: [{ type: String }],
});

const UserItems = mongoose.model<IUserItems>('UserItems', UserItemsSchema);

export default UserItems;