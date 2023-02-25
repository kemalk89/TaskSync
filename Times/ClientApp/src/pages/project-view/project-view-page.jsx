import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { formatDateTime } from "../../utils/date";

export const ProjectViewPage = () => {
  let { projectId } = useParams();
  const [project, setProject] = useState();

  useEffect(() => {
    const fetchProject = async () => {
      const response = await fetch("/api/project/" + projectId);
      if (response.status === 200) {
        const responseBody = await response.json();
        setProject(responseBody);
      } else {
        // ERR
      }
    };

    fetchProject();
  }, [projectId]);

  if (!project) {
    return <div>...</div>;
  }

  return (
    <div>
      <h1>{project.title}</h1>
      <p>
        <small>created at {formatDateTime(project.createdDate)}</small>
      </p>
      <p>{project.description}</p>
    </div>
  );
};
