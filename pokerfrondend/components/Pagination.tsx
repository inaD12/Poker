'use client';

import ReactPaginate from 'react-paginate';
import { useRouter } from 'next/navigation';

interface PaginationProps {
  totalCount: number;
  currentPage: number;
  route: string;
  pageSize: number;
}

export default function Pagination({
  totalCount,
  currentPage,
  route,
  pageSize
}: PaginationProps) {
  const router = useRouter();

  const handlePageClick =({ selected }: { selected: number }) => {
    const selectedPage = selected + 1;
    router.push(`/${route}/${selectedPage}`);
  };

  return (
    <ReactPaginate
    breakLabel="..."
    nextLabel="Next >"
    previousLabel="< Prev"
    onPageChange={handlePageClick}
    pageRangeDisplayed={5}
    pageCount={Math.ceil(totalCount / pageSize)}
    forcePage={currentPage - 1}
    renderOnZeroPageCount={null}
    containerClassName="flex items-center justify-center gap-2 mt-4"
    pageClassName="list-none"
    pageLinkClassName="block px-4 py-2 border border-gray-300 rounded-md text-black hover:bg-gray-200 cursor-default"
    activeLinkClassName="bg-[#E3DE61] text-black font-semibold cursor-default"
    previousClassName="list-none"
    previousLinkClassName="block px-4 py-2 border border-gray-300 rounded-md text-black hover:bg-gray-200 cursor-default"
    nextClassName="list-none"
    nextLinkClassName="block px-4 py-2 border border-gray-300 rounded-md text-black hover:bg-gray-200 cursor-default"
    breakClassName="list-none"
    breakLinkClassName="block px-4 py-2 text-gray-500 cursor-default"
    disabledLinkClassName="opacity-50 pointer-events-none cursor-default"
    />
  );
}
