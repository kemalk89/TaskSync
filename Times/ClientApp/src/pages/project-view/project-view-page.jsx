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
          created at {formatDateTime(fetchProject.data.createdDate)}
        </small>
      </p>
      <p>{fetchProject.data.description}</p>
    </div>
  );
};
