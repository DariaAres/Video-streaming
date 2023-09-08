import { Participant } from "./participant";

export interface FullRoom {
    id: number;
    title: string;
    fileName: string;
    videoStarted: boolean;
    movieTitle: string;
    canPlay: boolean;

    participants: Participant[];
}