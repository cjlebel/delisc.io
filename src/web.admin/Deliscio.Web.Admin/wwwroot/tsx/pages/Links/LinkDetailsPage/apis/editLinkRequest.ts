export type EditLinkRequest = {
    linkId: string,
    title: string,
    description?: string | null,
    domain: string,
    tags?: string[] | null
}