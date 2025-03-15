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

const courseScoresSchema: Schema = new mongoose.Schema({
    courseId: {type: String, required: true},
    userData: [UserScoresSchema]
});

const CourseScores = mongoose.model<ICourseScores>('CourseScores', courseScoresSchema);

export default CourseScores;