import { useState } from "react";
import { AsyncTypeahead } from "react-bootstrap-typeahead";
import { useQuery } from "react-query";
import { UserName } from "../user-name/user-name";

export const AutoCompleteAsync = ({
  id,
  apiFn,
  onChange,
  isMultiple = false,
  labelKey,
}) => {
  const [selectedOptions, setSelectedOptions] = useState([]);
  const [searchText, setSearchText] = useState("");
  const searchUsers = useQuery({
    queryKey: ["searchUsers", searchText],
    queryFn: () => apiFn(searchText),
    enabled: searchText.length > 0,
  });

  const filterBy = (option) => {
    const found = selectedOptions.find((o) => o.id === option.id);
    return !found;
  };

  const handleChange = (options) => {
    setSelectedOptions(options);
    if (isMultiple) {
      onChange(options);
    } else {
      onChange(options[0]);
    }
  };

  return (
    <AsyncTypeahead
      labelKey={labelKey}
      filterBy={filterBy}
      id={id}
      multiple={isMultiple}
      isLoading={searchUsers.isLoading}
      onSearch={(query) => setSearchText(query)}
      onChange={handleChange}
      options={searchUsers.data}
      renderMenuItemChildren={(item) => {
        return <UserName user={item} />;
      }}
    />
  );
};
