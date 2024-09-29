import Image from "next/image";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { getUser } from "@/actions/user";

export default async function ProfilePage() {
  const user = await getUser();

  if (!user) {
    return null;
  }

  return (
    <div className="container mx-auto p-4 w-full">
      <Card className="max-w-3xl mx-auto">
        <CardHeader className="flex flex-col sm:flex-row items-center gap-4">
          <Image
            src={user.image ?? "/avatar-placeholder.webp"}
            alt={user.name || "avatar"}
            width={128}
            height={128}
            className="rounded-full"
          />
          <div className="text-center sm:text-left">
            <CardTitle className="text-2xl font-bold">{user.name}</CardTitle>
            <p className="text-muted-foreground">@{user.id}</p>
          </div>
        </CardHeader>
        <CardContent>
          <div className="grid gap-4">
            <div>
              <h3 className="font-semibold mb-2">Email</h3>
              <p>{user.email}</p>
            </div>
            <div>
              <h3 className="font-semibold mb-2">Session Expired In</h3>
              <p>
                {new Date(
                  new Date(user.expires).getTime() + 2 * 60 * 60 * 1000
                ).toLocaleString("en-US", {
                  day: "2-digit",
                  month: "short",
                  year: "numeric",
                  hour: "numeric",
                  minute: "numeric",
                  hour12: true,
                })}
              </p>
            </div>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
