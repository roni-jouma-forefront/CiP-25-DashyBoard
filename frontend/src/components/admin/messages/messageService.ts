import type { MessageUI } from "../../../types/message.types";

const messageService = () => {
  const mockMessages: MessageUI[] = [
    {
      id: "1",
      messageScope: "all",
      title: "Muffins for breakfast",
      content: "Today we offer muffins for breakfast at the front desk.",
      status: "posted",
      postDateTime: "26-01-25 - 06:00",
      deleteDateTime: "26-01-25 - 06:00",
    },
    {
      id: "2",
      messageScope: "checkingOut",
      title: "Thank you for your stay",
      content:
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
      status: "pending",
      postDateTime: "26-01-25 - 06:00",
      deleteDateTime: "26-01-25 - 10:00",
    },
  ];

  return mockMessages;
};

export default messageService;