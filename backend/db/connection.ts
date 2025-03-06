import mongoose, { ConnectOptions } from "mongoose";

const connectDB = async (delay = 1000) => {
    const mongo_uri = process.env.MONGO_CONNECTION_STRING as string;

    while(true) {
        try {
            await mongoose.connect(mongo_uri, {
                useNewUrlParser: true,
                useUnifiedTopology: true,
            } as ConnectOptions);
            console.log("Connected to MongoDB!")
            return;
        } catch (err) {
            console.error(`Failed to connect to MongoDB. Retrying...\n${err}`);
            await new Promise((resolve) => setTimeout(resolve, delay));
            delay = Math.min(2 * delay, 5000);
        }
    }
}

export default connectDB;