import { Button, ButtonGroup, Table } from "reactstrap";

export const ItemsTable = ({ columns, rows, actions }) => {
  const getFieldValue = (column, item) => {
    if (typeof column.fieldName === "function") {
      return column.fieldName(item);
    } else {
      return item[column.fieldName];
    }
  };

  return (
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
        {rows?.map((item) => (
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
        ))}
      </tbody>
    </Table>
  );
};
