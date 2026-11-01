using Application.Features.InvoiceLists.Queries;


namespace Application.Features.InvoiceLists.Validators
{
    public static class GetInvoicesListQueryValidator
    {
        public static void Validate(GetInvoicesListQuery query)
        {
            if (query.PageNumber < 1)
                throw new ArgumentException("شماره صفحه باید حداقل ۱ باشد.", nameof(query.PageNumber));

            if (query.PageSize < 1 || query.PageSize > 100)
                throw new ArgumentException("تعداد آیتم‌ها در هر صفحه باید بین ۱ تا ۱۰۰ باشد.", nameof(query.PageSize));
        }
    }
}
