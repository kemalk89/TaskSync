import { useQuery } from "react-query";
import { useParams } from "react-router-dom";
import { api } from "../../api/api";
import { formatDateTime } from "../../utils/date";

export const ProjectViewPage = () => {
  let { projectId } = useParams();

  const fetchProject = useQuery({
    queryKey: ["project"],
    queryFn: () => api.fetchProject(projectId),
  });

  if (fetchProject.isLoading) {
    return <div>...</div>;
  }

  return (
    <div>
      <h1>{fetchProject.data.title}</h1>
      <p>
        <small>
          created at {formatDateTime(fetchProject.data.createdDate)} by{" "}
          {fetchProject.data.createdBy?.username}
        </small>
      </p>
      <h2>Description</h2>
      <p>{fetchProject.data.description}</p>
    </div>
  );
};
