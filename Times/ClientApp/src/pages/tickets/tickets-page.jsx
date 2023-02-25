import { Link, useNavigate } from "react-router-dom";
import { api } from "../../api/api";
import { TablePage } from "../../components/table-page/table-page";
import { TicketForm } from "./ticket-form";

export const TicketsPage = () => {
  const navigate = useNavigate();
  const tableData = {
    columns: [
      { title: "ID", fieldName: "id" },
      {
        title: "Project",
        fieldName: (item) => {
          return (
            <Link to={`/project/${item.project.id}`}>{item.project.title}</Link>
          );
        },
      },
      { title: "Title", fieldName: "title" },
    ],
  };

  return (
    <TablePage
      pageTitle="Tickets"
      cacheKey="tickets"
      getItemsApi={api.fetchTickets}
      saveItemApi={api.saveTicket}
      deleteItemApi={api.deleteTicket}
      onViewItem={(item) => navigate(`/ticket/${item.id}`)}
      itemForm={TicketForm}
      tableData={tableData}
    ></TablePage>
  );
};
