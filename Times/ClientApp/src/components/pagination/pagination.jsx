import {
  Pagination as BsPagination,
  PaginationItem,
  PaginationLink,
} from "reactstrap";

export const Pagination = ({ paged, onPageSelected }) => {
  const totalPages = Math.ceil(paged.total / paged.pageSize);

  const renderPages = () => {
    const elements = [];
    for (let i = 0; i < totalPages; i++) {
      elements.push(
        <PaginationItem key={`page-${i}`} active={paged.pageNumber === i + 1}>
          <PaginationLink onClick={() => onPageSelected(i + 1)}>
            {i + 1}
          </PaginationLink>
        </PaginationItem>
      );
    }
    return elements;
  };

  const onFirstPageSelected = () => {
    onPageSelected(1);
  };

  const onLastPageSelected = () => {
    onPageSelected(totalPages);
  };

  const onPreviousPageSelected = () => {
    let pageNumber = paged.pageNumber;
    if (pageNumber > 1) {
      pageNumber--;
    }

    onPageSelected(pageNumber);
  };

  const onNextPageSelected = () => {
    let pageNumber = paged.pageNumber;
    if (pageNumber < totalPages) {
      pageNumber++;
    }

    onPageSelected(pageNumber);
  };

  if (totalPages <= 1) {
    return null;
  }

  return (
    <BsPagination>
      <PaginationItem>
        <PaginationLink first onClick={onFirstPageSelected}></PaginationLink>
      </PaginationItem>
      <PaginationItem>
        <PaginationLink previous onClick={onPreviousPageSelected} />
      </PaginationItem>

      {renderPages()}

      <PaginationItem>
        <PaginationLink next onClick={onNextPageSelected} />
      </PaginationItem>
      <PaginationItem>
        <PaginationLink last onClick={onLastPageSelected}></PaginationLink>
      </PaginationItem>
    </BsPagination>
  );
};
