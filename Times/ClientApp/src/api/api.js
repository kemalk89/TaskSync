let accessToken = null;
let authConfig = null;
let accessTokenLoaderFn = undefined;

const initAccesToken = async() => {
    if (! accessToken) {
        accessToken = await accessTokenLoaderFn({
            audience: authConfig.audience
        });
        console.log(accessToken);
    }
}

const create = async (url, body) => {
    await initAccesToken();

    const res = await fetch(url, {
        body: JSON.stringify(body),
        method: 'POST',
        headers: {
            'content-type': 'application/json',
            'Authorization': `Bearer ${accessToken}`
        }
    });

    return await res.json();
};

const read = async (url) => {
    await initAccesToken();

    const res = await fetch(url, {
        headers: {
            'Authorization': `Bearer ${accessToken}`
        }
    });
    return await res.json();
};

const remove = async (url) => {
    await initAccesToken();

    return await fetch(url, {
        method: 'DELETE',
        headers: {
            'Authorization': `Bearer ${accessToken}`
        }
    });
};

export const api = {
    setAccessTokenLoader: (fn, config) => {
        authConfig = config;
        accessTokenLoaderFn = fn;
    },
    fetchTicket: async (ticketId) => {
        return read(`/api/ticket/${ticketId}`);
    },
    fetchTickets: async ({ pageNumber, pageSize }) => {
        return read(`/api/ticket?pageNumber=${pageNumber}&pageSize=${pageSize}`);
    },
    saveTicket: async (ticket) => {
        return create('/api/ticket', ticket);
    },
    deleteTicket: async (ticketId) => {
        return remove('/api/ticket/' + ticketId);
    },
    fetchProject: async (projectId) => {
        return read(`/api/project/${projectId}`);
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