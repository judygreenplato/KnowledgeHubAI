export interface Article {
    id: string;
    title: string;
    content: string;
    summary: string;
    isPublished: boolean;
    categoryId: string;
    createdAtUtc: string;
}