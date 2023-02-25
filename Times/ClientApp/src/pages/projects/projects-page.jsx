import { useNavigate } from "react-router-dom";
import { api } from "../../api/api";
import { TablePage } from "../../components/table-page/table-page";
import { ProjectForm } from "./project-form";

export const ProjectsPage = () => {
  const navigate = useNavigate();

  const tableData = {
    columns: [
      { title: "ID", fieldName: "id" },
      { title: "Title", fieldName: "title" },
    ],
  };

  return (
    <TablePage
      pageTitle="Projects"
      cacheKey="projects"
      getItemsApi={api.fetchProjects}
      saveItemApi={api.saveProject}
      deleteItemApi={api.deleteProject}
      itemForm={ProjectForm}
      tableData={tableData}
      onViewItem={(item) => navigate(`/project/${item.id}`)}
    ></TablePage>
  );
};
