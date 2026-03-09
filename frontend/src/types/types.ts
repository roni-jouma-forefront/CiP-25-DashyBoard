export type RoomStatus = "available" | "occupied";
export type Title = "Mrs" | "Ms" | "Mr" | "Mx" | null;

export type Room = {
  id: string;
  number: number;
  status: RoomStatus;
  title: Title;
  guestFirstName: string | null;
  guestLastName: string | null;
  flight: string | null;
};

export type AdditionalGuest = {
  firstName: string;
  lastName: string;
};

export type Staff = {
  name: string
}
