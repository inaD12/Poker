const apiRoutes = {
    baseUrl: 'http://localhost:5000',
    users: {
        login: `/api/users/login`,
        register: `api/users/register`,
        updateCurrent: `/api/users/update-current`,
        update: (id: string) => `/api/users/update${id}`,
        getAll: `/api/users/get-all`,
        get: (id: string) => `/api/users/get${id}`,
        deleteCurrent: `/api/users/delete-current`,
        delete: (id: string) => `/api/users/delete${id}`
    }
};

export default apiRoutes;