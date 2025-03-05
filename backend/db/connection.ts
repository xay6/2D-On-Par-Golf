import mongoose, { ConnectOptions } from "mongoose";
import * as dotenv from "dotenv";

dotenv.config({ path: __dirname + '/.env' });

const connectDB = async () => {
    try {
        await mongoose.connect(process.env.MONGO_DB_CONNECTION_STRING as string, {
            useNewUrlParser: true,
            useUnifiedTopology: true,
        } as ConnectOptions);
        console.log("Connected to MongoDB!")
    } catch (err) {
        console.error(`Failed to connect to MongoDB.\n${err}`); // Todo: Tentative error handling.
        process.exit();
    }
}

export default connectDB;