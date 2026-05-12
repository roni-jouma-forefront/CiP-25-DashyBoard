import { useState } from "react";
import type { Room } from "../types/types";

type RoomFilters = {
  roomNumber: string;
  guestName: string;
  flightNumber: string;
};

const initialFilters: RoomFilters = {
  roomNumber: "",
  guestName: "",
  flightNumber: "",
};

export const useRoomFilter = (rooms: Room[]) => {
  const [filterOpen, setFilterOpen] = useState(false);
  const [filters, setFilters] = useState<RoomFilters>(initialFilters);

  const filteredRooms = rooms.filter((room) => {
    const roomNumberMatch = String(room.roomNumber)
      .toLowerCase()
      .includes(filters.roomNumber.toLowerCase());
    const guestNameMatch =
      `${room.activeBooking?.guest?.firstName ?? ""} ${room.activeBooking?.guest?.lastName ?? ""}`
        .toLowerCase()
        .includes(filters.guestName.toLowerCase());
    const flightMatch = (room.activeBooking?.flightNumber ?? "")
      .toLowerCase()
      .includes(filters.flightNumber.toLowerCase());

    return roomNumberMatch && guestNameMatch && flightMatch;
  });

  const handleFilterChange = (field: keyof RoomFilters, value: string) => {
    setFilters((prev) => ({ ...prev, [field]: value }));
  };

  const clearFilters = () => {
    setFilters(initialFilters);
  };

  const toggleFilter = () => {
    setFilterOpen((prev) => !prev);
  };

  return {
    filteredRooms,
    filters,
    filterOpen,
    handleFilterChange,
    clearFilters,
    toggleFilter,
  };
};
