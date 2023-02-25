import { format } from "date-fns";

export const formatDateTime = (dateTimeString) => {
    return format(new Date(dateTimeString), "PPPpp");
}