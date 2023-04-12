import { useState } from "react";
import { Button, ButtonGroup } from "reactstrap";

export const EditableField = ({ renderValue, renderEditMode, onSave }) => {
  const [showEditBtn, setShowEditBtn] = useState(false);
  const [editMode, setEditMode] = useState(false);

  return (
    <>
      <div
        className="d-flex"
        onMouseOver={() => setShowEditBtn(true)}
        onMouseLeave={() => setShowEditBtn(false)}
      >
        {editMode ? renderEditMode() : renderValue()}
        <div className="ps-1">
          {editMode && (
            <ButtonGroup size="sm">
              <Button onClick={() => setEditMode(false)}>Cancel</Button>
              <Button color="primary" onClick={onSave}>
                Save
              </Button>
            </ButtonGroup>
          )}
          <Button
            className={showEditBtn && !editMode ? "visible" : "invisible"}
            onClick={() => setEditMode(true)}
            size="sm"
          >
            Edit
          </Button>
        </div>
      </div>
    </>
  );
};
