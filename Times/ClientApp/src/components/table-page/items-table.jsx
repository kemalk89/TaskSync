import { Button, ButtonGroup, Table } from "reactstrap";

export const ItemsTable = ({ columns, rows, actions, isLoading }) => {
  const getFieldValue = (column, item) => {
    if (typeof column.fieldName === "function") {
      return column.fieldName(item);
    } else {
      return item[column.fieldName];
    }
  };

  const renderRows = () => {
    if (rows?.length > 0) {
      return rows.map((item) => (
        <tr key={`t-${item.id}`}>
          {columns.map((c, i) => (
            <td key={i}>{getFieldValue(c, item)}</td>
          ))}
          <td>
            <ButtonGroup>
              {actions.map((a, i) => (
                <Button key={i} onClick={() => a.onClick(item)}>
                  {a.label}
                </Button>
              ))}
            </ButtonGroup>
          </td>
        </tr>
      ));
    }

    return null;
  };

  const renderLoading = () => {
    if (isLoading) {
      return (
        <div className="d-flex justify-content-center">
          <div className="spinner-border text-secondary spinner-border-sm" role="status">
            <span className="visually-hidden">Loading...</span>
          </div>
        </div>

      );
    } else if (!rows || rows.length === 0) {
      return <p className="text-center">No data found...</p>;
    }
  }

  return (
    <>
      <Table>
        <thead>
          <tr>
            {columns.map((c, i) => (
              <td key={i}>{c.title}</td>
            ))}
            <td></td>
          </tr>
        </thead>
        <tbody>
          {renderRows()}
      </tbody>
    </Table>
      {renderLoading()}
    </>
  );
};
