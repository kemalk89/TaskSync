export const addItemToPagedResult = (pagedResult, item) => {
    const clone = {
        ...pagedResult,
        total: pagedResult.total + 1,
        items: [...pagedResult.items, item]
    };

    return clone;
};
