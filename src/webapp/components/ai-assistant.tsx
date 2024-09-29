"use client";

import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { ScrollArea } from "@/components/ui/scroll-area";
import {
  Card,
  CardHeader,
  CardTitle,
  CardContent,
  CardFooter,
} from "@/components/ui/card";
import { MessageCircle, Send, XIcon } from "lucide-react";

interface Message {
  id: string;
  content: string;
  role: "user" | "assistant";
}

export default function AIAssistant() {
  const [isOpen, setIsOpen] = useState(false);
  const [messages, setMessages] = useState<Message[]>([]);
  const [input, setInput] = useState("");

  const toggleChat = () => setIsOpen(!isOpen);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!input.trim()) return;

    const userMessage: Message = {
      id: Date.now().toString(),
      content: input,
      role: "user",
    };
    setMessages((prev) => [...prev, userMessage]);
    setInput("");

    try {
      const response = await fetch("/api/chat", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ message: input }),
      });

      if (!response.ok) throw new Error("Failed to get response");

      const data = await response.json();

      const assistantMessage: Message = {
        id: Date.now().toString(),
        content: data.message,
        role: "assistant",
      };
      setMessages((prev) => [...prev, assistantMessage]);
    } catch (error) {
      console.error("Error:", error);
    }
  };

  if (!isOpen) {
    return (
      <Button
        className="fixed bottom-4 right-4 rounded-full p-4 shadow-lg"
        onClick={toggleChat}
      >
        <MessageCircle className="h-6 w-6" />
        <span className="sr-only">Open Chat</span>
      </Button>
    );
  }

  return (
    <Card className="fixed bottom-5 right-5 w-96 h-128 shadow-lg flex flex-col z-50">
      <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2 border-b">
        <CardTitle className="text-lg font-medium">Chat Assistant</CardTitle>
        <Button variant="ghost" size="sm" onClick={toggleChat}>
          <XIcon className="h-5 w-5" />
          <span className="sr-only">Close Chat</span>
        </Button>
      </CardHeader>
      <CardContent className="flex-grow overflow-y-auto p-4 h-80">
        <ScrollArea className="h-full w-full pr-4">
          {messages.map((message) => (
            <div
              key={message.id}
              className={`mb-4 ${
                message.role === "user" ? "text-right" : "text-left"
              }`}
            >
              <div
                className={`inline-block p-3 rounded-lg ${
                  message.role === "user"
                    ? "bg-blue-500 text-white"
                    : "bg-gray-200"
                }`}
              >
                {message.content}
              </div>
            </div>
          ))}
        </ScrollArea>
      </CardContent>
      <CardFooter className="border-t p-4">
        <form onSubmit={handleSubmit} className="flex w-full gap-2">
          <Input
            value={input}
            onChange={(e) => setInput(e.target.value)}
            placeholder="Type your message..."
            className="flex-grow"
          />
          <Button type="submit" size="icon">
            <Send className="h-5 w-5" />
            <span className="sr-only">Send</span>
          </Button>
        </form>
      </CardFooter>
    </Card>
  );
}
