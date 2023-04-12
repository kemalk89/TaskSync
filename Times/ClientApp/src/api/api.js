let accessToken = null;
let authConfig = null;
let accessTokenLoaderFn = undefined;

const initAccesToken = async() => {
    if (! accessToken) {
        accessToken = await accessTokenLoaderFn({
            audience: authConfig.audience
        });
    }
}

const post = async (url, body) => {
    await initAccesToken();

    const res = await fetch(url, {
        body: JSON.stringify(body),
        method: 'POST',
        headers: {
            'content-type': 'application/json',
            'Authorization': `Bearer ${accessToken}`
        }
    });

    if (!res.ok) {
        throw new Error(`Network error on URL ${url}: ${res.status}.`);
    }

    return res.json();
};

const get = async (url) => {
    await initAccesToken();

    const res = await fetch(url, {
        headers: {
            'Authorization': `Bearer ${accessToken}`
        }
    });

    if (!res.ok) {
        throw new Error(`Network error on URL ${url}: ${res.status}.`);
    }

    return res.json();
};

const remove = async (url) => {
    await initAccesToken();

    const res = await fetch(url, {
        method: 'DELETE',
        headers: {
            'Authorization': `Bearer ${accessToken}`
        }
    });

    if (!res.ok) {
        throw new Error(`Network error on URL ${url}: ${res.status}.`);
    }

    return res;
};

export const api = {
    setAccessTokenLoader: (fn, config) => {
        authConfig = config;
        accessTokenLoaderFn = fn;
    },
    fetchTicket: async (ticketId) => {
        return get(`/api/ticket/${ticketId}`);
    },
    fetchTickets: async ({ pageNumber, pageSize }) => {
        return get(`/api/ticket?pageNumber=${pageNumber}&pageSize=${pageSize}`);
    },
    saveTicket: async (ticket) => {
        return post('/api/ticket', ticket);
    },
    deleteTicket: async (ticketId) => {
        return remove('/api/ticket/' + ticketId);
    },
    fetchProject: async (projectId) => {
        return get(`/api/project/${projectId}`);
    },
    fetchProjects: async ({ pageNumber, pageSize }) => {
        return get(`/api/project?pageNumber=${pageNumber}&pageSize=${pageSize}`);
    },
    saveProject: async (project) => {
        return post('/api/project', project);
    },
    deleteProject: async (projectId) => {
        return remove('/api/project/' + projectId);
    },
    searchUsers: async (searchText) => {
        return post('/api/user', {
            searchText
        });
    }
};