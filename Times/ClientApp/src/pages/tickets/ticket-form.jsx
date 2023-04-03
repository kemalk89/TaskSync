import { Formik } from "formik";
import { Form, FormFeedback, FormGroup, Input, Label } from "reactstrap";
import { AutoCompleteAsync } from "../../components/autocomplete-async/autocomplete-async";
import { api } from "../../api/api";

export const TicketForm = ({ formId, saveHandler }) => {
  return (
    <Formik
      initialValues={{
        title: "",
        description: "",
        projectId: "",
        assignee: null,
      }}
      validate={(values) => {
        const errors = {};
        if (!values.title) {
          errors.title = "Required";
        }
        if (!values.projectId) {
          errors.projectId = "Required";
        }
        return errors;
      }}
      onSubmit={(values) => {
        saveHandler(values);
      }}
    >
      {({
        values,
        errors,
        touched,
        handleSubmit,
        handleChange,
        handleBlur,
        setFieldValue,
      }) => (
        <Form onSubmit={handleSubmit} id={formId}>
          <FormGroup>
            <Label for="projectId">Project</Label>
            <Input
              id="projectId"
              name="projectId"
              value={values.projectId}
              onChange={handleChange}
              onBlur={handleBlur}
              invalid={errors.projectId && touched.projectId}
            />
            <FormFeedback>{errors.projectId}</FormFeedback>
          </FormGroup>
          <FormGroup>
            <Label for="title">Title</Label>
            <Input
              id="title"
              name="title"
              value={values.title}
              onChange={handleChange}
              onBlur={handleBlur}
              invalid={errors.title && touched.title}
            />
            <FormFeedback>{errors.title}</FormFeedback>
          </FormGroup>
          <FormGroup>
            <Label for="assignee">Assignees</Label>
            <AutoCompleteAsync
              labelKey="username"
              id="assignee"
              apiFn={api.searchUsers}
              onChange={(selectedUser) =>
                setFieldValue("assignee", selectedUser)
              }
            />
          </FormGroup>
          <FormGroup>
            <Label for="description">Description</Label>
            <Input
              id="description"
              name="description"
              type="textarea"
              value={values.description}
              onChange={handleChange}
            />
          </FormGroup>
        </Form>
      )}
    </Formik>
  );
};
