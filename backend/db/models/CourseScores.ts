import mongoose, { Document, Schema } from "mongoose";

export interface IUserScores extends Document {
    user: mongoose.Types.ObjectId;
    score: Number;
}

export interface ICourseScores extends Document {
    courseId: String;
    userData: IUserScores[];
}

const UserScoresSchema: Schema = new mongoose.Schema({
    user: {type: mongoose.Types.ObjectId, ref: "User"},
    score: {type: Number, required: true}
});

const CourseScoresSchema: Schema = new mongoose.Schema({
    courseId: {type: String, required: true},
    userData: [UserScoresSchema]
});

export const CourseScores = mongoose.model<ICourseScores>('CourseScores', CourseScoresSchema);
export const UserScores = mongoose.model<IUserScores>('UserScores', UserScoresSchema);

// export default { CourseScores, UserScores };