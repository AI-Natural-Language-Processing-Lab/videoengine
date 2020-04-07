using System;
using System.Collections;
using System.Text;
namespace Jugnoon.Utility
{
    /// <summary>
    /// Pagination Script. 
    /// Provide utility classes for handling pagination in two different ways (i: Normal Pagination, ii: Advance Pagination).
    /// </summary>
    public class PaginationUtil
    {
        public enum Types
        {
            Normal = 0,
            Advance = 1,
            Simple = 2
        };

        /// <summary>
        /// Generate pagination links based on various parameters
        /// </summary>
        /// <param name="total_pages"></param>
        /// <param name="total_links"></param>
        /// <param name="selected_page"></param>
        /// <param name="pagination_type"></param>
        /// <returns></returns>
        public static ArrayList preparePagination(int total_pages, int total_links, int selected_page, Types pagination_type)
        {
            switch (pagination_type)
            {
                case Types.Advance:
                    return prepareAdvanceLinks(total_pages, selected_page);
                case Types.Simple:
                    return new ArrayList();
                default:
                    return prepareNormalLinks(total_pages, total_links, selected_page);
            }
        }

        /// <summary>
        /// Generate normal pagination links
        /// </summary>
        /// <param name="TotalPages"></param>
        /// <param name="Total_Links"></param>
        /// <param name="SelectedPage"></param>
        /// <returns></returns>
        public static ArrayList prepareNormalLinks(int TotalPages, int Total_Links, int SelectedPage)
        {
            int i;
            ArrayList arr = new ArrayList();
            if (TotalPages < Total_Links)
            {
                for (i = 1; i <= TotalPages; i++)
                {
                    arr.Add(i);
                }
            }
            else
            {
                int startindex = SelectedPage;
                int lowerbound = startindex - (int)Math.Floor((double)Total_Links / 2);
                int upperbound = startindex + (int)Math.Floor((double)Total_Links / 2);
                if (lowerbound < 1)
                {
                    //calculate the difference and increment the upper bound
                    upperbound = upperbound + (1 - lowerbound);
                    lowerbound = 1;
                }
                //if upperbound is greater than total page is
                if (upperbound > TotalPages)
                {
                    //calculate the difference and decrement the lower bound
                    lowerbound = lowerbound - (upperbound - TotalPages);
                    upperbound = TotalPages;
                }
                for (i = lowerbound; i <= upperbound; i++)
                {
                    arr.Add(i);
                }


            }
            return arr;

        }

        /// <summary>
        /// Generate advance pagination links
        /// </summary>
        /// <param name="TotalPages"></param>
        /// <param name="SelectedPage"></param>
        /// <returns></returns>
        public static ArrayList prepareAdvanceLinks(int TotalPages, int SelectedPage)
        {
            int i = 0;
            int value = 0;
            ArrayList arr = new ArrayList();
            ArrayList lower_arr = new ArrayList();
            ArrayList upper_arr = new ArrayList();

            int[] indexer = { 4, 40, 50, 400, 500, 4000, 5000, 40000, 50000 };
            int[] patter = { 1, 1, 1, 4, 40, 50, 400, 500, 4000, 5000, 40000, 50000 };

            if (SelectedPage == 1)
            {
                // 15 links
                for (i = 1; i <= 16; i++)
                {
                    if (i <= 7)
                        value = i;
                    else
                        value = value + indexer[i - 8];
                    if (value > TotalPages)
                        value = TotalPages;
                    if (!arr.Contains(value))
                        arr.Add(value);
                }
            }
            if (SelectedPage > 1)
            {
                if (TotalPages <= 16)
                {
                    for (i = 1; i <= 16; i++)
                    {
                        value = i;
                        if (value > TotalPages)
                            value = TotalPages;
                        if (!arr.Contains(value))
                            arr.Add(value);
                    }
                }
                else
                {
                    for (i = 0; i <= 7; i++)
                    {
                        if (value == 0)
                            value = SelectedPage - patter[i];
                        else
                            value = value - patter[i];

                        if (value > 0)
                        {
                            if (!lower_arr.Contains(value) && value != SelectedPage)
                                lower_arr.Add(value);
                        }
                    }
                    value = 0;
                    for (i = 0; i <= 7; i++)
                    {
                        if (value == 0)
                            value = SelectedPage + patter[i];
                        else
                            value = value + patter[i];

                        if (value > TotalPages)
                            value = TotalPages;

                        if (!upper_arr.Contains(value) && value != SelectedPage)
                            upper_arr.Add(value);
                    }
                    //// add lower array values
                    for (i = 0; i <= lower_arr.Count - 1; i++)
                    {
                        int rev_index = (lower_arr.Count - 1) - i;
                        if (!arr.Contains(lower_arr[rev_index]))
                            arr.Add(lower_arr[rev_index]);
                    }
                    if (!arr.Contains(SelectedPage))
                        arr.Add(SelectedPage);

                    //// add upper array values
                    for (i = 0; i <= upper_arr.Count - 1; i++)
                    {
                        if (!arr.Contains(upper_arr[i]))
                            arr.Add(upper_arr[i]);
                    }
                }
            }
            return arr;
        }


    }
}
/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.md', which is part of this source code package.
 * Copyright 2007 - 2020 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
