import express from "express";
import { getRewards, updateCoins, updateRewards } from "../controllers/userItemsController";

const userItemsRouter = express.Router();

userItemsRouter.put("/update-coins", updateCoins);

userItemsRouter.put("/update-rewards", updateRewards);

userItemsRouter.get("/get-rewards", getRewards)

export default userItemsRouter;