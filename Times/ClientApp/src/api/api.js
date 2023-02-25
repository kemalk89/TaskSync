const create = async (url, body) => {
    const res = await fetch(url, {
        body: JSON.stringify(body),
        method: 'POST',
        headers: {
            'content-type': 'application/json'
        }
    });

    return await res.json();
};

const read = async (url) => {
    const res = await fetch(url);
    return await res.json();
};

const remove = async (url) => {
    return await fetch(url, {
        method: 'DELETE',
    });
};

export const api = {
    fetchTickets: async ({ pageNumber, pageSize }) => {
        return read(`/api/ticket?pageNumber=${pageNumber}&pageSize=${pageSize}`);
    },
    saveTicket: async (ticket) => {
        return create('/api/ticket', ticket);
    },
    deleteTicket: async (ticketId) => {
        return remove('/api/ticket/' + ticketId);
    },
    fetchProjects: async ({ pageNumber, pageSize }) => {
        return read(`/api/project?pageNumber=${pageNumber}&pageSize=${pageSize}`);
    },
    saveProject: async (project) => {
        return create('/api/project', project);
    },
    deleteProject: async (projectId) => {
        return remove('/api/project/' + projectId);
    }
};