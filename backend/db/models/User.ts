import mongoose, { Document, Schema } from "mongoose";

export interface IUser extends Document {
    username: string;
    password: String;
    createdAt?: Date;
}

const userSchema: Schema = new mongoose.Schema({
    username: {
        type: String,
        required: true,
        unique: true,
    },
    password: {
        type: String,
        required: true,
    },
    createdAt: {
        type: String,
        default: (new Date).toLocaleDateString('en-GB'),
    }
});

const User = mongoose.model<IUser>('User', userSchema);

export default User;