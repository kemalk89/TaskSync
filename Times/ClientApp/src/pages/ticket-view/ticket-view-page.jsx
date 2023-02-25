import { useQuery } from "react-query";
import { useParams } from "react-router-dom";
import { api } from "../../api/api";
import { formatDateTime } from "../../utils/date";

export const TicketViewPage = () => {
  let { ticketId } = useParams();

  const fetchTicket = useQuery({
    queryKey: ["ticket"],
    queryFn: () => api.fetchTicket(ticketId),
  });

  if (fetchTicket.isLoading) {
    return <div>...</div>;
  }

  return (
    <div>
      <h1>{fetchTicket.data.title}</h1>
      <p>
        <small>created at {formatDateTime(fetchTicket.data.createdDate)}</small>
      </p>
      <p>{fetchTicket.data.description}</p>
    </div>
  );
};
