
import { ProjectViewPage } from "./pages/project-view/project-view-page";
import { ProjectsPage } from "./pages/projects/projects-page";
import { TicketsPage } from "./pages/tickets/tickets-page";

const AppRoutes = [
  {
    index: true,
    element: <ProjectsPage />
  },
  {
    path: '/projects',
    element: <ProjectsPage />
  },
  {
    path: '/project/:projectId',
    element: <ProjectViewPage />
  },
  {
    path: '/tickets',
    element: <TicketsPage />
  }
];

export default AppRoutes;
