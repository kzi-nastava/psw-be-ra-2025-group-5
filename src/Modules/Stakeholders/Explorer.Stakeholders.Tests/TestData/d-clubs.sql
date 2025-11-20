INSERT INTO stakeholders.[Clubs] (
    [Name], [Description], [Images], [CreatorId]
)
VALUES (
    'Planinari',
    'Planinarski klub',
    (
        SELECT CAST(N'' AS XML).value(
            'xs:base64Binary("iVBORw0KGgoAAAANSUhEUgAAAaQAAAGkCAIAAADxLsZiAAAF+0lEQVR4nOzXQQ3bQBBA0aYKjJAyCYMyCZMygIXQQ6Weqyrdlf3fIzAjjfLjfY8xfgA83c/VCwDMIHZAgtgBCWIHJIgdkCB2QILYAQliBySIHZAgdkCC2AEJYgckiB2QIHZAgtgBCWIHJIgdkCB2QILYAQliBySIHZAgdkCC2AEJYgckiB2QIHZAgtgBCWIHJIgdkDCe9qkaz+nzQJu5HNsE6b4sgMSxA5IEDsgQeyA..." 
            )',
            'varbinary(max)'
        )
    ),
    -11
);
